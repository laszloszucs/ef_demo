<template>
  <q-page class="flex flex-center">
    <div class="q-pa-md">
      <!-- <q-table
        title="Blogs"
        :data="data"
        :columns="columns"
        row-key="blogId"
        :pagination.sync="pagination"
        :loading="loading"
        :filter="filter"
        @request="onRequest"
        dark
        color="amber"
      > -->
      <q-table
        title="Blogs"
        :data="data"
        :columns="columns"
        :loading="loading"
      >
        <!-- <template v-slot:top-right>
          <q-input borderless dense debounce="300" v-model="filter" placeholder="Search">
            <template v-slot:append>
              <q-icon name="search" />
            </template>
          </q-input>
        </template> -->
      </q-table>
  </div>
  </q-page>
</template>

<script>
export default {
  data () {
    return {
      filter: '',
      loading: true,
      pagination: {
        sortBy: 'desc',
        descending: false,
        page: 1,
        rowsPerPage: 3,
        rowsNumber: 10
      },
      columns: [
        {
          name: 'Id',
          label: 'Id',
          field: 'blogId',
          sortable: true
        },
        {
          name: 'Url',
          label: 'Url',
          field: 'url',
          sortable: true
        },
        {
          name: 'Rating',
          label: 'Rating',
          field: 'rating',
          format: val => `${val} / 5`,
          sortable: true
        },
      ],
      data: []
    }
  },
  mounted() {
    this.loadData({
      pagination: this.pagination,
      filter: undefined
    })
  },
  methods: {
    loadData () {
      this.loading = true;
      this.$axios.get('/blog')
        .then((response) => {
          this.data = response.data
          this.loading = false;
        })
        .catch(() => {
          this.$q.notify({
            color: 'negative',
            position: 'top',
            message: 'Loading failed',
            icon: 'report_problem'
          });
          this.loading = false;
        });
    },
    // onRequest (props) {
    //   const { page, rowsPerPage, sortBy, descending } = props.pagination
    //   const filter = props.filter

    //   this.loading = true

    //   // emulate server
    //   setTimeout(() => {
    //     // update rowsCount with appropriate value
    //     this.pagination.rowsNumber = this.getRowsNumberCount(filter)

    //     // get all rows if "All" (0) is selected
    //     const fetchCount = rowsPerPage === 0 ? this.pagination.rowsNumber : rowsPerPage

    //     // calculate starting row of data
    //     const startRow = (page - 1) * rowsPerPage

    //     // fetch data from "server"
    //     const returnedData = this.fetchFromServer(startRow, fetchCount, filter, sortBy, descending)

    //     // clear out existing data and add new
    //     this.data.splice(0, this.data.length, ...returnedData)

    //     // don't forget to update local pagination object
    //     this.pagination.page = page
    //     this.pagination.rowsPerPage = rowsPerPage
    //     this.pagination.sortBy = sortBy
    //     this.pagination.descending = descending

    //     // ...and turn of loading indicator
    //     this.loading = false
    //   }, 1500)
    // },
  }
}
</script>