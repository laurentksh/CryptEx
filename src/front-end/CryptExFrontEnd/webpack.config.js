module.exports = {
  module: {
    rules: [
      {
        test: /\.scss$/,
        loader: 'postcss-loader',
        options: {
          postcssOptions: {
            ident: 'postcss',
            syntax: 'postcss-scss',
            plugins: [
              require('postcss-import'),
              require('tailwindcss'), // Nvm it's actually required now ?? Makes no sense... Comment this line if having issues with styling
              require('autoprefixer'),
            ],
          },
        },
      },
    ],
  },
};
