import { BrowserRouter, Route, Routes } from "react-router";
import {
  AppLayout,
  CreatePost,
  EditPost,
  GuestLayout,
  Login,
  PostDetails,
  Posts,
  ProtectedLayout,
  Register,
  RootPage,
  Users,
  UserDetails,
  EditUser,
} from "./pages";

function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        {/* protection logic inside rootpage */}
        <Route path="/" element={<RootPage />} />

        {/* guest routes */}
        <Route element={<GuestLayout />}>
          <Route path="/auth/login" element={<Login />} />
          <Route path="/auth/register" element={<Register />} />
        </Route>

        {/* protected routes */}
        <Route element={<ProtectedLayout />}>
          <Route element={<AppLayout />}>
            <Route path="/posts" element={<Posts />} />
            <Route path="/posts/create" element={<CreatePost />} />
            <Route path="/posts/:id" element={<PostDetails />} />
            <Route path="/posts/:id/edit" element={<EditPost />} />

            <Route path="/users" element={<Users />} />
            <Route path="/users/:id" element={<UserDetails />} />
            <Route path="/users/:id/edit" element={<EditUser />} />
          </Route>
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default AppRouter;
