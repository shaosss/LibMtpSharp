git clone https://github.com/gpg/libgcrypt.git
cd libgcrypt/
git checkout libgcrypt-1.9.3

./autogen.sh
./configure --host=$1 --with-gpg-error-prefix=$2 --prefix=$2 --disable-asm
make
if test -f cipher/gost-s-box.exe; then
    mv cipher/gost-s-box.exe cipher/gost-s-box
    make
fi
make install
cd ..