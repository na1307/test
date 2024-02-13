using Xunit.UserContext;

namespace Bluehill.Bcd.Test;

public class BcdStoreTest {
    [Fact]
    public void SystemStoreTest1() {
        _ = BcdStore.SystemStore;
    }

    [UserFact(NonAdminUsername, NonAdminPassword)]
    public void SystemStoreTest2() {
        Assert.Throws<RequiresAdministratorException>(() => BcdStore.SystemStore);
    }

    [Fact]
    public void FilePathTest1() {
        Assert.Equal(string.Empty, BcdStore.SystemStore.FilePath);
    }

    [Fact]
    public void IsSystemStoreTest1() {
        Assert.True(BcdStore.SystemStore.IsSystemStore);
    }

    [Fact]
    public void CreateStoreTest1() {
        _ = BcdStore.CreateStore(Path.Combine(AppContext.BaseDirectory, "BCD"));
    }

    [Fact]
    public void CreateStoreTest2() {
        Assert.Throws<ArgumentException>(() => BcdStore.CreateStore(null!));
    }

    [Fact]
    public void CreateStoreTest3() {
        Assert.Throws<ArgumentException>(() => BcdStore.CreateStore(string.Empty));
    }

    [Fact]
    public void CreateStoreTest4() {
        Assert.Throws<BcdException>(() => BcdStore.CreateStore(Path.Combine(AppContext.BaseDirectory, "BCD")));
    }

    [UserFact(NonAdminUsername, NonAdminPassword)]
    public void CreateStoreTest5() {
        Assert.Throws<RequiresAdministratorException>(() => BcdStore.CreateStore(Path.Combine(AppContext.BaseDirectory, "ANOTHERBCD")));
    }

    [Fact]
    public void OpenStoreTest1() {
        _ = BcdStore.OpenStore(Path.Combine(AppContext.BaseDirectory, "BCD"));
    }

    [Fact]
    public void OpenStoreTest2() {
        Assert.Throws<ArgumentException>(() => BcdStore.OpenStore(null!));
    }

    [Fact]
    public void OpenStoreTest3() {
        Assert.Throws<ArgumentException>(() => BcdStore.OpenStore(string.Empty));
    }

    [Fact]
    public void OpenStoreTest4() {
        Assert.Throws<FileNotFoundException>(() => BcdStore.OpenStore("Not Exists File"));
    }

    [UserFact(NonAdminUsername, NonAdminPassword)]
    public void OpenStoreTest5() {
        Assert.Throws<RequiresAdministratorException>(() => BcdStore.OpenStore(Path.Combine(AppContext.BaseDirectory, "BCD")));
    }

    [Fact]
    public void FilePathTest2() {
        Assert.Equal($@"\??\{Path.Combine(AppContext.BaseDirectory, "BCD")}", BcdStore.OpenStore(Path.Combine(AppContext.BaseDirectory, "BCD")).FilePath);
    }

    [Fact]
    public void IsSystemStoreTest2() {
        Assert.False(BcdStore.OpenStore(Path.Combine(AppContext.BaseDirectory, "BCD")).IsSystemStore);
    }
}
