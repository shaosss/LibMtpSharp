git clone https://github.com/gpg/libgpg-error.git
cd libgpg-error/
git checkout libgpg-error-1.42

./autogen.sh
./configure --host=$1 $3 --prefix=$2
make
make install
cd ..