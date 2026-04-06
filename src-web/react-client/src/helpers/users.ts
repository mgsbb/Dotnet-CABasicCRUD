import type { TUsersQuery } from "../types/users";

export function updateParams(
  searchParams: URLSearchParams,
  setSearchParams: (p: URLSearchParams) => void,
  updates: Partial<TUsersQuery>,
) {
  const params = new URLSearchParams(searchParams);

  Object.entries(updates).forEach(([key, value]) => {
    params.set(key, value.toString());
  });

  setSearchParams(params);
}
