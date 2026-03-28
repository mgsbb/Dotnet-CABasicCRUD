import { useState } from "react";
import { useSearchParams } from "react-router";

export type PostsQuery = {
  searchTerm: string;
  page: number;
  pageSize: number;
  postOrderBy: string;
  sortDirection: string;
  userId: string;
};

export function getPostsQuery(searchParams: URLSearchParams): PostsQuery {
  return {
    searchTerm: searchParams.get("searchTerm") ?? "",
    page: Number(searchParams.get("page") ?? 1),
    pageSize: Number(searchParams.get("pageSize") ?? 10),
    postOrderBy: searchParams.get("postOrderBy") ?? "createdAt",
    sortDirection: searchParams.get("sortDirection") ?? "desc",
    userId: searchParams.get("userId") ?? "",
  };
}

function updateParams(
  searchParams: URLSearchParams,
  setSearchParams: (p: URLSearchParams) => void,
  updates: Record<string, string>,
) {
  const params = new URLSearchParams(searchParams);

  Object.entries(updates).forEach(([key, value]) => {
    params.set(key, value);
  });

  setSearchParams(params);
}

export default function PostsFiler() {
  const [searchTerm, setSetTerm] = useState("");

  const [searchParams, setSearchParams] = useSearchParams();

  const postsQuery = getPostsQuery(searchParams);

  const handleFilterSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    updateParams(searchParams, setSearchParams, { searchTerm });
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
        value={postsQuery.postOrderBy}
        onChange={(e) => {
          updateParams(searchParams, setSearchParams, {
            postOrderBy: e.target.value,
          });
        }}
        className="border border-gray-300 rounded-sm p-2"
      >
        <option value="createdAt">Created</option>
        <option value="updatedAt">Updated</option>
        <option value="title">Title</option>
      </select>

      <select
        value={postsQuery.sortDirection}
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
        value={postsQuery.pageSize}
        onChange={(e) => {
          updateParams(searchParams, setSearchParams, {
            pageSize: e.target.value,
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
