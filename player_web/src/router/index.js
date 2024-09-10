import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import SearchResults from '../views/SearchResults.vue'
const routes = [
  {
    path: '/',
    component: HomeView
  },
  {
    name: 'search',
    path: '/search/:query',
    component: SearchResults,
    props: true
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
