import Vue from "vue";
import VueRouter from "vue-router";
import SignIn from "../views/SignIn";
import DocumentViewer from "../views/DocumentViewer";

Vue.use(VueRouter);

const routes = [
  {
    path: "/SignIn",
    name: "SignIn",
    component: SignIn
  },
  {
    path: "/Viewer",
    name: "Viewer",
    component: DocumentViewer
  },
];

const router = new VueRouter({
    mode: 'history',
  routes
});

export default router;
