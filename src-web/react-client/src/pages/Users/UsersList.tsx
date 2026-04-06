import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import { Link, useSearchParams } from "react-router";
import UsersFilter, { getUsersQuery } from "./components/UsersFilter";
import type { TUser } from "../../types/users";

// ===================================================================================================================
// ===================================================================================================================

export default function Users() {
  const [searchParams] = useSearchParams();

  const usersQuery = getUsersQuery(searchParams);

  // -------------------------------------------------------------------------------------------------------------------

  const {
    data: users,
    isLoading,
    isError,
    error,
  } = useQuery({
    queryKey: ["users", usersQuery],

    queryFn: async (): Promise<TUser[]> => {
      const response = await axios.get(
        `/api/v1/users?searchTerm=${usersQuery.searchTerm}&page=${usersQuery.page}&pageSize=${usersQuery.pageSize}&postOrderBy=${usersQuery?.userOrderBy}&sortDirection=${usersQuery?.sortDirection}`,
        {
          withCredentials: true,
        },
      );
      return response.data;
    },
  });

  // -------------------------------------------------------------------------------------------------------------------

  if (isLoading) return <div>Loading...</div>;

  if (isError) return <div>Error: {error.message}</div>;

  // -------------------------------------------------------------------------------------------------------------------

  return (
    <section className="px-2 sm:px-10 pb-10 w-full flex flex-col gap-10">
      {/*  title  */}
      <div className="flex gap-10 items-center">
        <h1 className="font-bold text-3xl">Users</h1>
      </div>
      {/*  filter form */}

      <UsersFilter />

      {/*  list of users  */}
      <ul className="w-3/4 flex flex-col gap-6">
        {users?.map((user) => {
          return (
            <li key={user.id} className="">
              <div className="flex items-start justify-between">
                <Link to={`/users/${user.id}`} className="w-full">
                  <p className="text-gray-700 font-bold">{user.name}</p>
                  <p className="w-2/3">{user.email}</p>
                </Link>
              </div>
            </li>
          );
        })}
      </ul>
    </section>
  );
}
