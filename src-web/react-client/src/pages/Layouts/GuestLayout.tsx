import { Navigate, Outlet } from "react-router";
import { useAuth } from "../../hooks/useAuth";

export default function GuestLayout() {
  const { isLoading, isError } = useAuth();

  if (isLoading) return <div>Loading...</div>;

  if (!isError) return <Navigate to="/" replace />;

  return <Outlet />;
}
