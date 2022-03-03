brew update

brew install pkg-config autoconf libtool libudev-dev make gettext git

sh $1/buildLibUsb.sh x86_64-apple-darwin $2
sh $1/buildLibgpg-error.sh x86_64-apple-darwin $2
sh $1/buildLibgcrypt.sh x86_64-apple-darwin $2
sh $1/buildIconv.sh x86_64-apple-darwin $2
sh $1/buildLibmtp.sh x86_64-apple-darwin $2