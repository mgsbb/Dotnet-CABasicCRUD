import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import { Link, useSearchParams } from "react-router";
import PostsFilter, { getPostsQuery } from "./PostsFilter";
import Post from "./Post";

export type TPost = {
  id: string;
  title: string;
  content: string;
  userId: string;
  authorName: string;
  createdAt: string;
  updatedAt: string;
};

export default function Posts() {
  const [searchParams] = useSearchParams();

  const postsQuery = getPostsQuery(searchParams);

  const {
    data: posts,
    isLoading,
    isError,
    error,
  } = useQuery({
    queryKey: ["posts", postsQuery],

    queryFn: async (): Promise<TPost[]> => {
      const response = await axios.get(
        `/api/v1/posts?searchTerm=${postsQuery.searchTerm}&page=${postsQuery.page}&pageSize=${postsQuery.pageSize}&postOrderBy=${postsQuery?.postOrderBy}&sortDirection=${postsQuery?.sortDirection}`,
        {
          withCredentials: true,
        },
      );
      return response.data;
    },
  });

  if (isLoading) return <div>Loading...</div>;

  if (isError) return <div>Error: {error.message}</div>;

  return (
    <section className="px-4 sm:px-10 flex flex-col gap-10">
      {/* title and button  */}
      <div className="flex gap-10 items-center">
        <h1 className="font-bold text-3xl">Posts</h1>

        <nav className="">
          <Link to="/posts/create">
            <button className="bg-gray-800 text-white py-2 px-4 rounded-sm cursor-pointer text-sm">
              Create new
            </button>
          </Link>
        </nav>
      </div>

      <PostsFilter />

      {/* list of posts  */}
      <ul className="mt-10 flex flex-col gap-10 pb-10">
        {posts?.map((post) => {
          return <Post post={post} key={post.id} />;
        })}
      </ul>
    </section>
  );
}
