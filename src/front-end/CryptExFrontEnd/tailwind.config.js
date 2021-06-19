module.exports = {
  prefix: '',
  purge: {
    enabled: true,
    layers: ['base', 'components', 'uitlities', 'forms'],
    content: [
      './src/**/*.html',
      './src/**/*.ts',
      './src/**/*.scss',
    ]
  },
  darkMode: 'class', // or 'media' or 'class'
  mode: 'jit', //JIT compilation, disable if any styling issue occurs.
  theme: {
    extend: {
      screens: {
        'phone': '320px'
      },
      colors: {
        'blue-logo': '#167DEA',
      },
      keyframes: {
        'fade-in': {
          '0%': {
            opacity: '0'
          },
          '100%': {
            opacity: '1'
          }
        },
        'fade-out': {
          '0%': {
            opacity: '1'
          },
          '100%': {
            opacity: '0'
          }
        },
        'w-right': {
          '0%': {
            width: '0%'
          },
          '25%': {
            width: '25%'
          },
          '50%': {
            width: '50%'
          },
          '75%': {
            width: '75%'
          },
          '100%': {
            width: '100%'
          },
        }
      },
      animation: {
        'fade-in': 'fade-in 0.5 ease-out',
        'fade-out': 'fade-out 0.5 ease-out',
        'w-right': 'w-right 0.5 ease-out'
      }
    },
  },
  /*variants: {
    extend: {
      ringOffsetWidth: ['hover', 'active'],
      ringOffsetColor: ['hover', 'active'],
      ringColor: ['hover', 'active'],
      ringWidth: ['hover', 'active'],
    },
  },*/
  plugins: [
    require('@tailwindcss/forms')
  ],
};
