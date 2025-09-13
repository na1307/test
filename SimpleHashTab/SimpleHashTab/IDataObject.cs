namespace SimpleHashTab;

[GeneratedComInterface]
[Guid("0000010e-0000-0000-C000-000000000046")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal unsafe partial interface IDataObject {
    STGMEDIUM GetData(in FORMATETC pformatetcIn);

    [PreserveSig]
    int GetDataHere(in FORMATETC pformatetc, ref STGMEDIUM pmedium);

    void QueryGetData(in FORMATETC pformatetc);

    STGMEDIUM GetCanonicalFormatEtc(in FORMATETC pformatectIn);

    void SetData(in FORMATETC pformatetc, in STGMEDIUM pmedium, [MarshalAs(UnmanagedType.Bool)] bool fRelease);

    void* EnumFormatEtc(uint dwDirection);

    uint DAdvise(in FORMATETC pformatetc, uint advf, void* pAdvSink);

    void DUnadvise(uint dwConnection);

    void** EnumDAdvise();
}
