import { parseISO, format } from "date-fns";
import mk from "date-fns/locale/mk";

import Vue from "vue";


Vue.filter("prefixDiff", function(value) {
  if (value > 0) {
    return `+${value}`;
  } else {
    return `${value}`;
  }
});


Vue.filter("formatDate", function(value, fmt) {
  
  if (!value) {
    return ""
  }

  if (!fmt) {
    fmt = "d. MMMM"
  }

  let date = null

  if (value instanceof Date) {
    date = value
  } else if (typeof(value) === 'number') {
    date = new Date(value)
  } else {
    date = parseISO(value)
  }

  return format(date, fmt, {
    locale: mk
  });

});