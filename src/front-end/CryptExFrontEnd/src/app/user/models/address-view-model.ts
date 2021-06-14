import { CountryViewModel } from "src/app/api/models/country-view-model";

export interface AddressViewModel  {
  street: string;
  city: string;
  postalCode: string;
  country: CountryViewModel;
}
