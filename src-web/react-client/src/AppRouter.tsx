import { BrowserRouter, Route, Routes } from "react-router";
import { Login } from "./pages";

function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/auth/login" element={<Login />}></Route>
      </Routes>
    </BrowserRouter>
  );
}

export default AppRouter;
