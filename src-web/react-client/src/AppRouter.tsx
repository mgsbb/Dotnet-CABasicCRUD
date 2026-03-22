import { BrowserRouter, Route, Routes } from "react-router";
import {
  AppLayout,
  CreatePost,
  GuestLayout,
  Login,
  Posts,
  ProtectedLayout,
  Register,
  RootPage,
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
          </Route>
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default AppRouter;
