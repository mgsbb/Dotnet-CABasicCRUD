export type TPost = {
  id: string;
  title: string;
  content: string;
  userId: string;
  authorName: string;
  createdAt: string;
  updatedAt: string;
};

export type TComment = {
  id: string;
  body: string;
  postId: string;
  userId: string;
  authorName: string;
  createdAt: string;
  updatedAt: string;
};

export type TPostsQuery = {
  searchTerm: string;
  page: number;
  pageSize: number;
  postOrderBy: string;
  sortDirection: string;
  userId: string;
};
