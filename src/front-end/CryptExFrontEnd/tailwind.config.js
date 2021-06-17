module.exports = {
  purge: ['./src/**/*.{html,ts,scss}'],
  darkMode: false, // or 'media' or 'class'
  mode: 'jit', //JIT compilation, disable if any styling issue occurs.
  theme: {
    extend: {
      screens: {
        'phone': '320px'
      },
      colors: {
        'blue-logo': '#167DEA',
      },
      /*minHeight: {
        //'0': '0', //Already provided by Tailwind
        '1/4': '25%',
        '1/2': '50%',
        '3/4': '75%',
        //'full': '100%', //Already provided by Tailwind
        'h-0.5': '0.125rem',
        '1': '0.25rem',
        '1.5': '0.375rem',
        '2': '0.5rem',
        '2.5': '0.625rem',
        '3': '0.75rem',
        '3.5': '0.875rem',
        '4': '1rem',
        '5': '1.25rem',
        '6': '1.5rem',
        '7': '1.75rem',
        '8': '2rem',
        '9': '2.25rem',
        '10': '2.5rem',
        '11': '2.75rem',
        '12': '3rem',
        '14': '3.5rem',
        '16': '4rem',
        '20': '5rem',
        '24': '6rem',
        '28': '7rem',
        '32': '8rem',
        '36': '9rem',
        '40': '10rem',
        '44': '11rem',
        '48': '12rem',
        '52': '13rem',
        '56': '14rem',
        '60': '15rem',
        '64': '16rem',
        '72': '18rem',
        '80': '20rem',
        '96': '24rem',
      },
      maxHeight: {
        //'0': '0', //Already provided by Tailwind
        '1/4': '25%',
        '1/2': '50%',
        '3/4': '75%',
        //'full': '100%', //Already provided by Tailwind
        'h-0.5': '0.125rem',
        '1': '0.25rem',
        '1.5': '0.375rem',
        '2': '0.5rem',
        '2.5': '0.625rem',
        '3': '0.75rem',
        '3.5': '0.875rem',
        '4': '1rem',
        '5': '1.25rem',
        '6': '1.5rem',
        '7': '1.75rem',
        '8': '2rem',
        '9': '2.25rem',
        '10': '2.5rem',
        '11': '2.75rem',
        '12': '3rem',
        '14': '3.5rem',
        '16': '4rem',
        '20': '5rem',
        '24': '6rem',
        '28': '7rem',
        '32': '8rem',
        '36': '9rem',
        '40': '10rem',
        '44': '11rem',
        '48': '12rem',
        '52': '13rem',
        '56': '14rem',
        '60': '15rem',
        '64': '16rem',
        '72': '18rem',
        '80': '20rem',
        '96': '24rem',
      },
      minWidth: {
        //'0': '0', //Already provided by Tailwind
        '1/4': '25%',
        '1/2': '50%',
        '3/4': '75%',
        //'full': '100%', //Already provided by Tailwind
        'h-0.5': '0.125rem',
        '1': '0.25rem',
        '1.5': '0.375rem',
        '2': '0.5rem',
        '2.5': '0.625rem',
        '3': '0.75rem',
        '3.5': '0.875rem',
        '4': '1rem',
        '5': '1.25rem',
        '6': '1.5rem',
        '7': '1.75rem',
        '8': '2rem',
        '9': '2.25rem',
        '10': '2.5rem',
        '11': '2.75rem',
        '12': '3rem',
        '14': '3.5rem',
        '16': '4rem',
        '20': '5rem',
        '24': '6rem',
        '28': '7rem',
        '32': '8rem',
        '36': '9rem',
        '40': '10rem',
        '44': '11rem',
        '48': '12rem',
        '52': '13rem',
        '56': '14rem',
        '60': '15rem',
        '64': '16rem',
        '72': '18rem',
        '80': '20rem',
        '96': '24rem',
      },
      maxWidth: {
        //'0': '0', //Already provided by Tailwind
        '1/4': '25%',
        '1/2': '50%',
        '3/4': '75%',
        //'full': '100%', //Already provided by Tailwind
        'h-0.5': '0.125rem',
        '1': '0.25rem',
        '1.5': '0.375rem',
        '2': '0.5rem',
        '2.5': '0.625rem',
        '3': '0.75rem',
        '3.5': '0.875rem',
        '4': '1rem',
        '5': '1.25rem',
        '6': '1.5rem',
        '7': '1.75rem',
        '8': '2rem',
        '9': '2.25rem',
        '10': '2.5rem',
        '11': '2.75rem',
        '12': '3rem',
        '14': '3.5rem',
        '16': '4rem',
        '20': '5rem',
        '24': '6rem',
        '28': '7rem',
        '32': '8rem',
        '36': '9rem',
        '40': '10rem',
        '44': '11rem',
        '48': '12rem',
        '52': '13rem',
        '56': '14rem',
        '60': '15rem',
        '64': '16rem',
        '72': '18rem',
        '80': '20rem',
        '96': '24rem',
      },*/
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
