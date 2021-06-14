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
              //require('tailwindcss'), // Fuck whoever made this file and added this line, I spent 2 hours debugging a bug caused by it.
              require('autoprefixer'),
            ],
          },
        },
      },
    ],
  },
};
