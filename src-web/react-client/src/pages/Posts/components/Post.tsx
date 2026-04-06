import { Link } from "react-router";
import type { TPost } from "../../../types/posts";

export default function Post({ post }: { post: TPost }) {
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
            <p className="text-gray-600 text-sm">{post.content}</p>
          </Link>
        </div>
      </div>
    </li>
  );
}
