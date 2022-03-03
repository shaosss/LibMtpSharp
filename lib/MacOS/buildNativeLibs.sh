brew update

brew install pkg-config
brew install autoconf
brew install libtool
brew install libudev-dev
brew install make
brew install gettext
brew install git
brew install automake

sh $1/buildLibUsb.sh x86_64-apple-darwin $2
sh $1/buildLibgpg-error.sh x86_64-apple-darwin $2
sh $1/buildLibgcrypt.sh x86_64-apple-darwin $2
sh $1/buildIconv.sh x86_64-apple-darwin $2
sh $1/buildLibmtp.sh x86_64-apple-darwin $2