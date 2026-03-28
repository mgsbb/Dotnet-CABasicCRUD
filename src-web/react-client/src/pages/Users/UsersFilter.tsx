import { useState } from "react";
import { useSearchParams } from "react-router";

export type UsersQuery = {
  searchTerm: string;
  page: number;
  pageSize: number;
  userOrderBy: string;
  sortDirection: string;
};

export function getUsersQuery(searchParams: URLSearchParams): UsersQuery {
  return {
    searchTerm: searchParams.get("searchTerm") ?? "",
    page: Number(searchParams.get("page") ?? 1),
    pageSize: Number(searchParams.get("pageSize") ?? 10),
    userOrderBy: searchParams.get("userOrderBy") ?? "createdAt",
    sortDirection: searchParams.get("sortDirection") ?? "desc",
  };
}

function updateParams(
  searchParams: URLSearchParams,
  setSearchParams: (p: URLSearchParams) => void,
  updates: Partial<UsersQuery>,
) {
  const params = new URLSearchParams(searchParams);

  Object.entries(updates).forEach(([key, value]) => {
    params.set(key, value.toString());
  });

  setSearchParams(params);
}

export default function UsersFilter() {
  const [searchTerm, setSetTerm] = useState("");

  const [searchParams, setSearchParams] = useSearchParams();

  const usersQuery = getUsersQuery(searchParams);

  const handleFilterSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    updateParams(searchParams, setSearchParams, { searchTerm: searchTerm });
  };

  return (
    <form
      onSubmit={handleFilterSubmit}
      className="flex flex-col lg:flex-row gap-2 text-sm w-full"
    >
      <input
        value={searchTerm}
        onChange={(e) => setSetTerm(e.target.value)}
        placeholder="Search posts..."
        className="border border-gray-300 rounded-sm p-2 flex-1"
      />

      <select
        value={usersQuery.userOrderBy}
        onChange={(e) => {
          updateParams(searchParams, setSearchParams, {
            userOrderBy: e.target.value,
          });
        }}
        className="border border-gray-300 rounded-sm p-2"
      >
        <option value="createdAt">Created</option>
        <option value="updatedAt">Updated</option>
        <option value="name">Name</option>
        <option value="email">Email</option>
      </select>

      <select
        value={usersQuery.sortDirection}
        onChange={(e) => {
          updateParams(searchParams, setSearchParams, {
            sortDirection: e.target.value,
          });
        }}
        className="border border-gray-300 rounded-sm p-2"
      >
        <option value="desc">Desc</option>
        <option value="asc">Asc</option>
      </select>

      <select
        value={usersQuery.pageSize}
        onChange={(e) => {
          updateParams(searchParams, setSearchParams, {
            pageSize: parseInt(e.target.value),
          });
        }}
        className="border border-gray-300 rounded-sm p-2"
      >
        <option value={10}>10/page</option>
        <option value={20}>20/page</option>
        <option value={30}>30/page</option>
      </select>

      <button
        type="submit"
        className="bg-gray-800 text-white py-2 px-4 rounded-sm cursor-pointer w-fit"
      >
        Filter
      </button>
    </form>
  );
}
