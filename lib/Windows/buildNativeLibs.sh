sudo apt-get update

sudo apt-get install mingw-w64 mingw-w64-tools autoconf libtool libudev-dev make gettext git -y

sh ./../buildLibUsb.sh x86_64-w64-mingw32 $1
sh ./../buildLibgpg-error.sh x86_64-w64-mingw32 $1 '--enable-threads=windows'
sh ./../buildLibgcrypt.sh x86_64-w64-mingw32 $1
sh ./../buildIconv.sh x86_64-w64-mingw32 $1
sh ./../buildLibmtp.sh x86_64-w64-mingw32 $1

cd ..