module.exports = {
  purge: ['./src/**/*.{html,ts,scss}'],
  darkMode: false, // or 'media' or 'class'
  theme: {
    extend: {
      colors: {
        'blue-logo': '#167DEA',
      }
    },
  },
  variants: {
    extend: {
      ringOffsetWidth: ['hover', 'active'],
      ringOffsetColor: ['hover', 'active'],
      ringColor: ['hover', 'active'],
      ringWidth: ['hover', 'active'],
    },
  },
  plugins: [
    require('@tailwindcss/forms')
  ],
};
