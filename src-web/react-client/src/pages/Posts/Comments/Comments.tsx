import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import { Link } from "react-router";
import DeleteComment from "./DeleteComment";
import { useAuth } from "../../../hooks/useAuth";
import type { TComment } from "../../../types/posts";

// ===================================================================================================================
// ===================================================================================================================

export default function Comments({ postId }: { postId?: string }) {
  const { data: currentUser } = useAuth();

  const {
    data: comments,
    isLoading: isCommentsLoading,
    isError: isCommentsError,
  } = useQuery({
    queryKey: ["posts", postId, "comments"],
    queryFn: async (): Promise<TComment[]> => {
      const response = await axios.get(`/api/v1/posts/${postId}/comments`, {
        withCredentials: true,
      });

      return response.data;
    },
  });

  // -------------------------------------------------------------------------------------------------------------------

  if (isCommentsLoading) return <div>Loading...</div>;

  if (isCommentsError) return <div>Error</div>;

  return (
    <section className="mt-16">
      <h4 className="text-2xl text-gray-700 font-semibold mb-8">
        Comments ({comments?.length})
      </h4>
      {comments?.length === 0 ? (
        <p className="text-gray-800">No comments yet.</p>
      ) : (
        <div className="flex flex-col gap-6">
          {comments?.map((comment) => {
            return (
              <div
                key={comment.id}
                className="flex flex-col md:flex-row gap-2 items-start md:justify-between"
              >
                <div>
                  <p className="mb-1">{comment.body}</p>
                  <Link
                    to={`/users/${comment.userId}`}
                    className="text-sm text-gray-600 font-medium"
                  >
                    by {comment.authorName}{" "}
                  </Link>
                  <small className="text-gray-500 font-medium">
                    at {comment.createdAt}
                  </small>
                </div>
                {comment.userId == currentUser?.id && (
                  <DeleteComment commentId={comment.id} postId={postId} />
                )}
              </div>
            );
          })}
        </div>
      )}
    </section>
  );
}
