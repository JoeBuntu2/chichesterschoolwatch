import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'thousandSuff'
})
export class ThousandSuffPipe implements PipeTransform {

  transform(input: any, args?: any): any {
 
    let suffixes = [' thousand', ' million'];

    if (Number.isNaN(input)) {
      return null;
    }

    if (input < 1000) {
      return input;
    }

    let exp = Math.floor(Math.log(input) / Math.log(1000));

    return (input / Math.pow(1000, exp)).toFixed(args) + suffixes[exp - 1];
  }
}
