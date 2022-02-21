git clone https://github.com/libusb/libusb.git
cd libusb/
git checkout v1.0.25

./autogen.sh --host=$1 --prefix=$2
make
make install
cd ..