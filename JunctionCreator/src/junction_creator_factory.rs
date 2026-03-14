use crate::DLL_REF_COUNT;
use crate::junction_creator::JunctionCreator;
use std::ffi::c_void;
use std::ptr::null_mut;
use std::sync::atomic::Ordering;
use windows::Win32::Foundation::{CLASS_E_NOAGGREGATION, E_POINTER, S_OK};
use windows::Win32::System::Com::{IClassFactory, IClassFactory_Impl};
use windows::core::*;

#[implement(IClassFactory)]
pub(crate) struct JunctionCreatorFactory;

impl Drop for JunctionCreatorFactory {
    fn drop(&mut self) {
        DLL_REF_COUNT.fetch_sub(1, Ordering::Relaxed);
    }
}

impl IClassFactory_Impl for JunctionCreatorFactory_Impl {
    fn CreateInstance(&self, punkouter: Ref<IUnknown>, riid: *const GUID, ppvobject: *mut *mut c_void) -> Result<()> {
        if ppvobject.is_null() {
            return Err(E_POINTER.into());
        }

        unsafe {
            *ppvobject = null_mut();
        }

        if !punkouter.is_null() {
            return Err(CLASS_E_NOAGGREGATION.into());
        }

        let instance = JunctionCreator::new().into_object();

        DLL_REF_COUNT.fetch_add(1, Ordering::Relaxed);

        let hr = unsafe { instance.QueryInterface(riid, ppvobject) };

        if !hr.eq(&S_OK) {
            // The Drop impl on HashPropSheet will decrement the count on failure,
            // so we don't need to do it manually here.
            return Err(hr.into());
        }

        Ok(())
    }

    fn LockServer(&self, flock: BOOL) -> Result<()> {
        match flock.as_bool() {
            true => {
                DLL_REF_COUNT.fetch_add(1, Ordering::Relaxed);
            }
            false => {
                DLL_REF_COUNT.fetch_sub(1, Ordering::Relaxed);
            }
        };

        Ok(())
    }
}
