import { useQuery } from "@tanstack/react-query";
import axios from "axios";

type Comment = {
  id: string;
  body: string;
  postId: string;
  userId: string;
  authorName: string;
  createdAt: string;
  updatedAt: string;
};

export default function Comments({ postId }: { postId?: string }) {
  const {
    data: comments,
    isLoading: isCommentsLoading,
    isError: isCommentsError,
  } = useQuery({
    queryKey: ["posts", postId, "comments"],
    queryFn: async (): Promise<Comment[]> => {
      const response = await axios.get(`/api/v1/posts/${postId}/comments`, {
        withCredentials: true,
      });

      return response.data;
    },
  });

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
                  <a
                    href="/users/@comment.UserId"
                    className="text-sm text-gray-600 font-medium"
                  >
                    by {comment.authorName}{" "}
                  </a>
                  <small className="text-gray-500 font-medium">
                    at {comment.createdAt}
                  </small>
                </div>
                <nav>
                  <a href="/comments/@comment.Id/delete">
                    <button className="bg-red-100 px-2 py-1 text-red-700 text-sm font-semibold rounded-sm cursor-pointer">
                      Delete
                    </button>
                  </a>
                </nav>
              </div>
            );
          })}
        </div>
      )}
    </section>
  );
}
