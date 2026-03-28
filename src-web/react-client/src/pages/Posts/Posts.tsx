import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import { Link } from "react-router";
import PostsFilter from "./PostsFilter";
import { useState } from "react";

export type Post = {
  id: string;
  title: string;
  content: string;
  userId: string;
  authorName: string;
  createdAt: string;
  updatedAt: string;
};

export type PostsQuery = {
  searchTerm: string;
  page: number;
  pageSize: number;
  postOrderBy: string;
  sortDirection: string;
  userId: string;
};

const initialPostQuery: PostsQuery = {
  searchTerm: "",
  page: 1,
  pageSize: 10,
  postOrderBy: "createdAt",
  sortDirection: "desc",
  userId: "",
};

export default function Posts() {
  const [filterFormData, setFilterFormData] =
    useState<PostsQuery>(initialPostQuery);

  const {
    data: posts,
    isLoading,
    isError,
    error,
  } = useQuery({
    queryKey: [
      "posts",
      {
        searchTerm: filterFormData.searchTerm,
        page: filterFormData.page,
        pageSize: filterFormData.pageSize,
        postOrderBy: filterFormData.postOrderBy,
        sortDirection: filterFormData.sortDirection,
        userId: filterFormData.userId,
      },
    ],

    queryFn: async (): Promise<Post[]> => {
      const response = await axios.get(
        `/api/v1/posts?searchTerm=${filterFormData.searchTerm}&page=${filterFormData?.page}&pageSize=${filterFormData?.pageSize}&postOrderBy=${filterFormData?.postOrderBy}&sortDirection=${filterFormData?.sortDirection}`,
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

      <PostsFilter
        filterFormData={filterFormData}
        setFilterFormData={setFilterFormData}
      />

      {/* list of posts  */}
      <ul className="mt-10 flex flex-col gap-10 pb-10">
        {posts?.map((post) => {
          return (
            <li key={post.id} className="">
              <div className="flex flex-col lg:flex-row gap-2 items-start justify-between">
                <div className="flex flex-col">
                  <div className="flex items-end gap-4">
                    <Link to={`/posts/${post.id}`} className="flex flex-col">
                      <p className="text-gray-700 font-bold">{post.title}</p>
                    </Link>
                    <Link to={`/users/${post.userId}`} className="">
                      <p className="text-sm font-bold text-gray-500 ">
                        - {post.authorName}
                      </p>
                    </Link>
                  </div>
                  <Link to={`/posts/${post.id}`} className="flex flex-col">
                    <p className="">{post.content}</p>
                  </Link>
                </div>
              </div>
            </li>
          );
        })}
      </ul>
    </section>
  );
}
