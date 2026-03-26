import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import { Link, useParams } from "react-router";
import AddComment from "./AddComment";
import Comments from "./Comments";
import { useAuth } from "../../hooks/useAuth";
import DeletePost from "./DeletePost";

type Post = {
  id: string;
  title: string;
  content: string;
  userId: string;
  authorName: string;
  createdAt: string;
  updatedAt: string;
};

export default function PostDetails() {
  const { id: postId } = useParams();

  const {
    data: post,
    isLoading: isPostLoading,
    isError: isPostError,
  } = useQuery({
    queryKey: ["posts", postId],
    queryFn: async (): Promise<Post> => {
      const response = await axios.get(`/api/v1/posts/${postId}`, {
        withCredentials: true,
      });

      return response.data;
    },
  });

  const { data: currentUser } = useAuth();

  if (isPostLoading) return <div>Loading...</div>;

  if (isPostError) return <div>Error occurred</div>;

  return (
    <article className="pb-10 sm:px-10">
      {/*  Post  */}
      <h1 className="font-bold text-3xl text-gray-700">{post?.title}</h1>
      {/* author name */}
      <Link to={`/users/${post?.userId}`}>
        <p className="font-bold text-gray-600 mt-2">
          <span className="text-sm text-gray-500">written by </span>
          {post?.authorName}
        </p>
      </Link>

      <nav className="flex gap-6 mt-6 items-center">
        {currentUser?.id == post?.userId && (
          <div className="flex gap-2">
            <Link to={`/posts/${post?.id}/edit`}>
              <button className="bg-gray-100 px-2 py-1 text-gray-700 font-semibold rounded-sm cursor-pointer">
                Edit
              </button>
            </Link>

            <DeletePost postId={post?.id} />
          </div>
        )}

        <Link to="/posts" className="text-gray-500 font-medium">
          Back to posts
        </Link>
      </nav>
      <p className="mt-8">{post?.content}</p>

      <AddComment postId={post?.id} />

      <Comments postId={post?.id} />
    </article>
  );
}
