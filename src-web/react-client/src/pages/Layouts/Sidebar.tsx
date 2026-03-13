import { useMutation } from "@tanstack/react-query";
import axios from "axios";
import { Link, useNavigate } from "react-router";
import { queryClient } from "../../App";

export default function Sidebar() {
  const navigate = useNavigate();

  const logoutMutation = useMutation({
    mutationFn: async () => {
      const response = await axios.post(
        "/api/v1/auth/logout",
        {},
        { withCredentials: true },
      );
      return response.data;
    },

    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["auth"] });
      navigate("/auth/login");
    },

    onError: (error: any) => {
      console.error(error);
    },
  });

  const handleLogoutSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    logoutMutation.mutate();
  };

  return (
    <aside
      id="sidebar"
      className="fixed left-0 top-0 h-screen bg-white border-r border-gray-300 p-4 transition-all duration-300 z-20 w-80"
    >
      {/* hamburger menu button to toggle sidebar */}
      <button
        type="button"
        // onclick="toggleSidebar()"
        className="text-gray-500 cursor-pointer"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 24 24"
          stroke-width="1.5"
          stroke="currentColor"
          className="size-8"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            d="M3.75 6.75h16.5M3.75 12h16.5m-16.5 5.25h16.5"
          />
        </svg>
      </button>

      {/* user details */}
      <div className="sidebar-user-details px-4 py-10 flex flex-col gap-4 items-center">
        {/* replace with user profile image  */}
        <div className="border border-gray-400 rounded-sm p-4 w-32 h-32"></div>
        {/* replace with username instead  */}
        <p className="font-bold text-gray-700">
          <a href="/users/@CurrentUser.UserId">@CurrentUser.Username</a>
        </p>
      </div>

      <div className="px-10 py-10 h-10 flex flex-col gap-8 text-gray-500 text-lg font-medium w-full">
        {/* home icon  */}
        <Link to="/" className="inline-flex gap-4">
          <svg
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            stroke-width="1.5"
            stroke="currentColor"
            className="size-6"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              d="m2.25 12 8.954-8.955c.44-.439 1.152-.439 1.591 0L21.75 12M4.5 9.75v10.125c0 .621.504 1.125 1.125 1.125H9.75v-4.875c0-.621.504-1.125 1.125-1.125h2.25c.621 0 1.125.504 1.125 1.125V21h4.125c.621 0 1.125-.504 1.125-1.125V9.75M8.25 21h8.25"
            />
          </svg>
          <span className="sidebar-label">Home</span>
        </Link>

        {/* posts  */}
        <Link to="/posts" className="inline-flex gap-4">
          <svg
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            stroke-width="1.5"
            stroke="currentColor"
            className="size-6"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              d="M12 7.5h1.5m-1.5 3h1.5m-7.5 3h7.5m-7.5 3h7.5m3-9h3.375c.621 0 1.125.504 1.125 1.125V18a2.25 2.25 0 0 1-2.25 2.25M16.5 7.5V18a2.25 2.25 0 0 0 2.25 2.25M16.5 7.5V4.875c0-.621-.504-1.125-1.125-1.125H4.125C3.504 3.75 3 4.254 3 4.875V18a2.25 2.25 0 0 0 2.25 2.25h13.5M6 7.5h3v3H6v-3Z"
            />
          </svg>
          <span className="sidebar-label">Posts</span>
        </Link>

        {/* users */}
        <Link to="/users" className="inline-flex gap-4">
          <svg
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            stroke-width="1.5"
            stroke="currentColor"
            className="size-6"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              d="M15 19.128a9.38 9.38 0 0 0 2.625.372 9.337 9.337 0 0 0 4.121-.952 4.125 4.125 0 0 0-7.533-2.493M15 19.128v-.003c0-1.113-.285-2.16-.786-3.07M15 19.128v.106A12.318 12.318 0 0 1 8.624 21c-2.331 0-4.512-.645-6.374-1.766l-.001-.109a6.375 6.375 0 0 1 11.964-3.07M12 6.375a3.375 3.375 0 1 1-6.75 0 3.375 3.375 0 0 1 6.75 0Zm8.25 2.25a2.625 2.625 0 1 1-5.25 0 2.625 2.625 0 0 1 5.25 0Z"
            />
          </svg>
          <span className="sidebar-label">Users</span>
        </Link>

        {/* logout */}
        <form
          onSubmit={handleLogoutSubmit}
          className="inline-flex gap-4 cursor-pointer"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            stroke-width="1.5"
            stroke="currentColor"
            className="size-6"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              d="M15.75 9V5.25A2.25 2.25 0 0 0 13.5 3h-6a2.25 2.25 0 0 0-2.25 2.25v13.5A2.25 2.25 0 0 0 7.5 21h6a2.25 2.25 0 0 0 2.25-2.25V15M12 9l-3 3m0 0 3 3m-3-3h12.75"
            />
          </svg>

          <button type="submit" className="cursor-pointer sidebar-label">
            Logout
          </button>
        </form>
      </div>
    </aside>
  );
}
