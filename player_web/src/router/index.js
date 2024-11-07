import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import SearchResultsView from '../views/SearchResultsView.vue'
import PerformerView from '../views/PerformerView.vue'
import PlaylistView from '../views/PlaylistView.vue'
import AddPlaylistView from '../views/AddPlaylistView.vue'
const routes = [
  {
    path: '/',
    component: HomeView
  },
  {
    name: 'search',
    path: '/search/:query',
    component: SearchResultsView,
    props: true
  },
  {
    name: 'performer',
    path: '/performer/:performerid',
    component: PerformerView,
    props: true
  },
  {
    name: 'playlist',
    path: '/playlist/:id',
    component: PlaylistView,
    props: true
  },
  {
    name: 'add-playlist',
    path: '/add-playlist',
    component: AddPlaylistView,
    props: false
  },
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
