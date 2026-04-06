import type { TPostsQuery } from "../types/posts";

export function getPostsQuery(searchParams: URLSearchParams): TPostsQuery {
  return {
    searchTerm: searchParams.get("searchTerm") ?? "",
    page: Number(searchParams.get("page") ?? 1),
    pageSize: Number(searchParams.get("pageSize") ?? 10),
    postOrderBy: searchParams.get("postOrderBy") ?? "createdAt",
    sortDirection: searchParams.get("sortDirection") ?? "desc",
    userId: searchParams.get("userId") ?? "",
  };
}

export function updateParams(
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
