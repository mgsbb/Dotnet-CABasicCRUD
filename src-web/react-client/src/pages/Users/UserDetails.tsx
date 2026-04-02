import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import { Link, useParams, useSearchParams } from "react-router";
import type { User } from "./Users";
import { useAuth } from "../../hooks/useAuth";
import PostsFilter, { getPostsQuery } from "../Posts/PostsFilter";
import Post from "../Posts/Post";
import type { TPost } from "../Posts/Posts";

export default function UserDetails() {
  const { id: userId } = useParams();

  const {
    data: user,
    isLoading,
    isError,
  } = useQuery({
    queryKey: ["users", userId],
    queryFn: async (): Promise<User> => {
      const response = await axios.get(`/api/v1/users/${userId}`, {
        withCredentials: true,
      });

      return response.data;
    },
  });

  const [searchParams] = useSearchParams();

  const postsQuery = getPostsQuery(searchParams);

  const {
    data: posts,
    isLoading: isPostsLoading,
    isError: isPostsError,
  } = useQuery({
    queryKey: ["posts", { ...postsQuery, userId }],

    queryFn: async (): Promise<TPost[]> => {
      const response = await axios.get(
        `/api/v1/posts?userId=${userId}&searchTerm=${postsQuery.searchTerm}&page=${postsQuery.page}&pageSize=${postsQuery.pageSize}&postOrderBy=${postsQuery?.postOrderBy}&sortDirection=${postsQuery?.sortDirection}`,
        {
          withCredentials: true,
        },
      );
      return response.data;
    },
  });

  const { data: currentUser } = useAuth();

  if (isLoading) return <div>Loading...</div>;

  if (isError) return <div>Error occurred</div>;

  return (
    <section className="flex flex-col gap-20 px-2 sm:px-10">
      <div className="flex flex-col xl:flex-row xl:justify-between gap-10 items-center xl:items-end ">
        <div className="flex flex-col xl:flex-row gap-10 items-start xl:items-end ">
          {/*  replace with profile photo  */}
          <div className="border border-gray-400 rounded-sm p-4 w-64 h-64"></div>
          {/* name and id  */}
          <div className="flex flex-col gap-2">
            <div className="flex items-end gap-10 justify-between">
              <h1 className="font-bold text-3xl text-gray-700">{user?.name}</h1>
              {user?.id === currentUser?.id && (
                <nav>
                  <Link
                    to={`/users/${currentUser?.id}/edit`}
                    className="text-gray-500 font-semibold"
                  >
                    Update
                  </Link>
                </nav>
              )}
            </div>

            <p>{user?.username}</p>
          </div>
        </div>
        {user?.id !== currentUser?.id && (
          <div className="">
            <form
              // asp-controller="Conversations"
              // asp-action="StartPrivateConversation"
              className="bg-blue-100 items-start"
            >
              <input type="hidden" name="targetUserId" value="@Model.Id" />
              <button
                type="submit"
                className="px-4 py-2 text-white bg-gray-900 rounded-sm cursor-pointer text-sm "
              >
                Message
              </button>
            </form>
          </div>
        )}
      </div>
      <div>
        <h2 className="text-sm font-bold text-gray-700 ">About</h2>
        <p>{user?.bio}</p>
      </div>
      <div className="flex flex-col gap-10">
        <h2 className="font-bold text-gray-600 text-2xl">Recent posts</h2>
        {/*  filter form  */}
        <PostsFilter />

        {/*  list of posts  */}
        {isPostsLoading ? (
          <>Loading...</>
        ) : isPostsError ? (
          <>Error...</>
        ) : (
          <ul className="mt-10 flex flex-col gap-10 pb-10">
            {posts?.map((post) => {
              return <Post post={post} key={post.id} />;
            })}
          </ul>
        )}
      </div>
    </section>
  );
}
