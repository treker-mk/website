<template>
  <div>
    <b-table
      responsive
      bordered
      outlined
      hover
      :stickyColumn="' '"
      :sort-desc="true"
      :sticky-header="tableHeight"
      :items="items"
      :fields="fields"
    ></b-table>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import _ from "lodash";

export default {
  props: ["tableHeight"],
  data() {
    return {
      items: [],
      fields: [],
      dimensions: [
        "tests.performed",
        "tests.performed.todate",
        "cases.confirmed",
        "cases.confirmed.todate",
        "cases.hs.employee.confirmed.todate",
        // SLO-spec "cases.rh.employee.confirmed.todate",
        // SLO-spec "cases.rh.occupant.confirmed.todate",
        "cases.unclassified.confirmed.todate",
        "cases.active",
        "cases.recovered.todate",
        "cases.closed.todate",
        "vaccination.administered.todate",
        "state.in_hospital",
        "state.icu",
        // SLO-spec "state.critical",
        "state.in_hospital.todate",
        // SLO-spec "state.out_of_hospital.todate",
        "state.deceased.todate",
      ],
      loaded: false
    };
  },
  watch: {
    tableData() {
      this.refreshData()
    }
  },
  computed: {
    ...mapGetters("tableData", ["tableData", "filterTableData"])
  },
  methods: {
    refreshData(){
      const { items, fields } = this.filterTableData(this.dimensions);
      this.items = items;
      this.fields = fields;
      this.loaded = true;
    }
  }
};
</script>
