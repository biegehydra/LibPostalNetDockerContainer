## Instructions to run locally

### Build or Download LibPostal.dll
Before using the .Net bindings, you must build the libpostal C library or download the precompiled library.

Downloads (From openvenues/libpostal AppVeyor CI):
+ [x86 / 32-bit version](https://goo.gl/Bf3EzE)
+ [x64 / 64-bit version](https://goo.gl/o8DAi8)

If you opt for downloading just extract the folder and put all its contents
in the appropriate folder. (LibPostalNet/x64 or LibPostalNet/x86) 

### Note
Downloading the LibPostal.dll doesn't mean you don't have to install LibPostal, it just
means you don't have to build it. You still need to install it on your machine.

### Install LibPostal on your machine

Instructions can be found on LibPostal's github https://github.com/openvenues/libpostal

These are the instructions I used on my WINDOWS machine. WHEREVER YOU INSTALL LIBPOSTAL
YOU MUST UPDATE THE APPSETTINGS.DEVELOPMENT.JSON FILE WITH THE APPROPRIATE PATH.

#### Installation (Windows)

**MSys2/MinGW**

For Windows the build procedure currently requires MSys2 and MinGW. This can be downloaded from http://msys2.org. Please follow the instructions on the MSys2 website for installation.

Please ensure Msys2 is up-to-date by running:
```
pacman -Syu
```

Install the following prerequisites:
```
pacman -S autoconf automake curl git make libtool gcc mingw-w64-x86_64-gcc
```

Then to build the C library:
```
git clone https://github.com/openvenues/libpostal
cd libpostal
cp -rf windows/* ./
./bootstrap.sh
./configure --datadir=[...some dir with a few GB of space...]
make -j4
make install
```
Notes: When setting the datadir, the `C:` drive would be entered as `/c`. The libpostal build script automatically add `libpostal` on the end of the path, so '/c' would become `C:\libpostal\` on Windows.

The compiled .dll will be in the `src/.libs/` directory and should be called `libpostal-1.dll`.

If you require a .lib import library to link this to your application. You can generate one using the Visual Studio `lib.exe` tool and the `libpostal.def` definition file:
```
lib.exe /def:libpostal.def /out:libpostal.lib /machine:x64
```

## Instructions to run docker container locally

When running this project in a docker container locally, you will want to take advantage of
layer caching; because installing LibPostal in a container takes a lot of time.
To do this, open command prompt in the root of the project (with the docker file) and run the following

```
docker build -t libpostalnet:v1 .
```
Then every time you create a new image, use the same name and it will use the cached layers.

```
docker build -t libpostalnet:v2 .
```

### Run the container

```
docker run -d -p {host port}:{container port} {image name}:{image tag}

ex: docker run -d -p 7052:80 libpostal:v6
```

After that, you can test that it's running by going to http://localhost:7052/health
Note: It is http not https here.
