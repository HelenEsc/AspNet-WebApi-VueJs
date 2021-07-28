<template>
  <div class="container-fluid mt-4">
    <h1 class="h1">List of Books</h1>
    <b-alert :show="loading" variant="info">Loading...</b-alert>
    <b-row>
      <b-col>
        <table class="table table-striped">
          <thead>
            <tr>
              <th>Title</th>
              <th>Date</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="book in books" :key="book.id">
              <td>{{ book.title}}</td>
              <td>{{ formatDate(book.date) }}</td>
              <td class="text-right">
                <a href="#" @click.prevent="updateBook(book)">Edit</a> -
                <a href="#" @click.prevent="deleteBook(book.id)">Delete</a>
              </td>
      	    </tr>
      	  </tbody>
        </table>
      </b-col >
      <b-col lg="3">
        <b-card :title="(model.id ? 'Edit Book ID#' + model.id : 'New Book')">
          <form @submit.prevent="createBook">
            <b-form-group label="Title">
              <b-form-input type="text" v-model="model.title"></b-form-input>
            </b-form-group>
            <b-form-group label="Date">
              <b-form-input rows="4" v-model="model.date" type="datetime-local"></b-form-input>
            </b-form-group>
            <div>
              <b-btn type="submit" variant="success">Save Record</b-btn>
            </div>
          </form>
        </b-card>
      </b-col>
    </b-row>
  </div>
</template>

<script>

import api from '@/WebApiService';
import moment from 'moment'

export default {
  data() {
    return {
      loading: false,
      books: [],
      model: {}
    };
  },
  async created() {
    this.getAll()
  },
  methods: {
    async getAll() {
      this.loading = true

      try {
        this.books = await api.getAll()
      } finally {
        this.loading = false
      }
    },
    async updateBook(book) {
      // you use Object.assign() to create a new (separate) instance
      this.model = Object.assign({}, book)
      this.model.date = this.formatLocalDate(this.model.date)
    },
    async createBook() {
      const isUpdate = !!this.model.id;

      if (isUpdate) {
        await api.update(this.model.id, this.model)
      } else {
        await api.create(this.model)
      }

      // Clear the data inside of the form
      this.model = {}

      // Fetch all books again to have latest data
      await this.getAll()
    },
    async deleteBook(id) {
      if (confirm('Are you sure you want to delete this book?')) {
        // if you are editing a book that you deleted, remove it from the form
        if (this.model.id === id) {
          this.model = {}
        }

        await api.delete(id)
        await this.getAll()
      }
    },
    formatDate(value){
        if (value) {
          var date = moment.utc(String(value)).format('MM/DD/YYYYThh:mm')
          return date.replace('T', ' ');
        }
    },
    formatLocalDate(value){
      if (value) {
        return moment.utc(String(value)).format('YYYY-MM-DDTHH:mm')
      }
    }
  }
}
</script>