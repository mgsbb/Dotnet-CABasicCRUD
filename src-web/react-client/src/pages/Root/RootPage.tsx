import { useAuth } from "../../hooks/useAuth";
import AppLayout from "../Layouts/AppLayout";
import Home from "./Home";
import Landing from "./Landing";

export default function RootPage() {
  const { isError, isLoading } = useAuth();

  if (isLoading) return <div>Loading...</div>;

  if (isError) return <Landing />;

  return (
    <AppLayout>
      <Home />
    </AppLayout>
  );
}
