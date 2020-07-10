import Vue from 'vue'
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
import { library } from '@fortawesome/fontawesome-svg-core'
import { faSpinner, faGlobe, faCaretDown } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

import 'whatwg-fetch'
// global vue filters
import './filters'

// this is common Vue initialization for index.js and embed.js (app entrypoints)

Vue.use(BootstrapVue)
Vue.use(IconsPlugin)

// fontawesome
library.add(faSpinner, faGlobe, faCaretDown)
Vue.component('font-awesome-icon', FontAwesomeIcon)

Vue.config.productionTip = false
