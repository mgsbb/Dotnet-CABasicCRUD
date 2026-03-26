import { Link } from "react-router";
import { useAuth } from "../../hooks/useAuth";

export default function CurrentUser() {
  const { data: currentUser, isLoading, isError } = useAuth();

  if (isLoading) return <>Loading...</>;

  if (isError) return <>Error...</>;

  return (
    <div className="sidebar-user-details px-4 py-10 flex flex-col gap-4 items-center">
      {/* replace with user profile image  */}
      <div className="border border-gray-400 rounded-sm p-4 w-32 h-32"></div>
      {/* replace with username instead  */}
      <p className="font-bold text-gray-700">
        <Link to={`/users/${currentUser?.id}`}>{currentUser?.username}</Link>
      </p>
    </div>
  );
}
