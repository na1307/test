macro_rules! define_errors_enum {
    { $enum_name:ident; $( $variant:ident = $value:expr, )* } => {
        #[derive(Debug, Copy, Clone, Eq, PartialEq)]
        #[repr(i32)]
        pub enum $enum_name {
            $(
            $variant = $value,
            )*
        }

        impl From<i32> for $enum_name {
            fn from(value: i32) -> Self {
                match value {
                    $(
                    $value => $enum_name::$variant,
                    )*
                    _ => panic!("Unknown value"),
                }
            }
        }

        impl From<$enum_name> for i32 {
            fn from(value: $enum_name) -> Self {
                value as i32
            }
        }
    };
}
