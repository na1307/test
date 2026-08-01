# EDGE Mods

This directory contains several Mods.

## How to build .dll mods

**Requirements**:

* [Xmake](https://xmake.io/) Build System
* Visual C++ (2026 Recommended)
* LLVM with Clang-CL compiler

First of all, you need to generate local package for `BluehillLoader`. Just run `generate.cmd`.

Then set the build directory by entering the following command:

```
xmake f
```

Then build the project:

```
xmake build
```

### Switching between debug and release

To switch to release mode:

```
xmake f -m release
```

To switch to debug mode:

```
xmake f -m debug
```
