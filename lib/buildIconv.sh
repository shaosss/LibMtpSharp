wget -c https://ftp.gnu.org/pub/gnu/libiconv/libiconv-1.16.tar.gz -O - | tar -xz
cd libiconv-1.16
./configure --host=$1 --prefix=$2
make
make install
cd ..