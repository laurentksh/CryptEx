# CryptEx-Frontend

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 10.1.1.

## Running locally

This project requires NodeJS to be installed (and thus NPM).
Before everything you must run 'npm install' in the current folder, so that dependencies get installed.
Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.
Add '--prod' when running the command above to use the Azure production server. (CORS is/should be configured properly so that you can use it when running locally).

## Common issues
Common issues you might encounter, and how to resolve them.

### No styling
If no styling is loaded (i.e all the pages are a big mess to look at), or you have any issue with CSS compilation,
then consider disabling Tailwind's JIT compilation in 'tailwind.config.js'. The property name is 'jit', setting it to false will resolve this issue.



## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `--prod` flag for a production build.