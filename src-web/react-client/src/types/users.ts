export type TUser = {
  id: string;
  name: string;
  email: string;
  username: string;
  createdAt: string;
  updatedAt: string;
  bio: string;
  profileImageUrl: string;
};

export type TUsersQuery = {
  searchTerm: string;
  page: number;
  pageSize: number;
  userOrderBy: string;
  sortDirection: string;
};
