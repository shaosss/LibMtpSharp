sudo apt-get update

sudo apt-get install mingw-w64 mingw-w64-tools autoconf libtool libudev-dev make gettext git -y

sh $1/buildLibUsb.sh x86_64-w64-mingw32 $2
sh $1/buildLibgpg-error.sh x86_64-w64-mingw32 $2 '--enable-threads=windows'
sh $1/buildLibgcrypt.sh x86_64-w64-mingw32 $2
sh $1/buildIconv.sh x86_64-w64-mingw32 $2
sh $1/buildLibmtp.sh x86_64-w64-mingw32 $2

cd ..