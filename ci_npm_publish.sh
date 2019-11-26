#!/bin/bash

cd coreclr-hosting
echo "//registry.npmjs.org/:_authToken=\${NPM_TOKEN}" > .npmrc

if [ "$TRAVIS_TAG" != "" ]
then
	echo "Publishing coreclr-hosting for tag $TRAVIS_TAG"
    npm version $TRAVIS_TAG --allow-same-version
	npm publish --access public --ignore-scripts
else
    echo "Dry run publishing coreclr-hosting for tag $TRAVIS_TAG"
    npm publish --access public --ignore-scripts --dry-run
fi

cd ../electron-blazor-glue
echo "//registry.npmjs.org/:_authToken=\${NPM_TOKEN}" > .npmrc

if [ "$TRAVIS_TAG" != "" ]
then
	echo "Publishing electron-blazor-glue for tag $TRAVIS_TAG"
    # npm uninstall coreclr-hosting --ignore-scripts
    # npm install @nodeclrhosting/coreclr-hosting@$TRAVIS_TAG --ignore-scripts
    sed -i 's_"coreclr-hosting": "file:../coreclr-hosting"_"@nodeclrhost/coreclr-hosting": "$TRAVIS_TAG"_g' package.json
    npm version $TRAVIS_TAG --allow-same-version
	npm publish --access public --ignore-scripts
else
    echo "Dry run publishing electron-blazor-glue for tag $TRAVIS_TAG"
    npm publish --access public --ignore-scripts --dry-run
fi

cd ..