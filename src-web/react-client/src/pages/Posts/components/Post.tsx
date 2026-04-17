import { Link } from "react-router";
import type { TPost } from "../../../types/posts";
import { useState } from "react";
import { timeAgo } from "../../../helpers/common";

export default function Post({ post }: { post: TPost }) {
  const [currentImageIndex, setCurrentImageIndex] = useState(0);

  return (
    <>
      <div className="rounded-lg border border-gray-300 p-4 space-y-4">
        {/* author info */}
        <Link to={`/users/${post.userId}`} className="flex items-center gap-3">
          <img
            src={
              post.authorProfileImageUrl ||
              "https://api.dicebear.com/7.x/initials/svg?seed=" +
                post.authorName
            }
            className="w-12 h-12 rounded-full object-cover"
          />
          <div className="flex flex-col">
            <span className="font-semibold text-sm">{post.authorName}</span>
            <span className="text-xs text-gray-500">
              {timeAgo(post.createdAt)}
            </span>
          </div>
        </Link>

        <Link to={`/posts/${post.id}`} className="flex gap-2 flex-col">
          {/* title */}
          <h2 className="text-2xl font-semibold ">{post.title}</h2>

          {/* content */}
          <p className="text-gray-700 text-sm">{post.content}</p>
        </Link>

        {/* image */}
        {post.mediaUrls.length > 0 && (
          <div className="relative group w-full h-87.5 overflow-hidden bg-gray-100 hover:bg-red-100">
            <img
              src={post.mediaUrls[currentImageIndex]}
              className="w-full h-full object-cover"
            />

            {/* overlay */}
            <div className="absolute inset-0 bg-black/0 group-hover:bg-black/50 transition duration-300" />

            {/* next/prev button */}
            {post.mediaUrls.length > 1 && (
              <>
                <button
                  onClick={() => {
                    setCurrentImageIndex((i) =>
                      i === 0 ? post.mediaUrls.length - 1 : i - 1,
                    );
                  }}
                  className="absolute left-2 top-1/2 -translate-y-1/2 bg-white/80  
                  text-black px-2 py-1 rounded-full"
                >
                  ‹
                </button>
                <button
                  onClick={() => {
                    setCurrentImageIndex((i) =>
                      i === post.mediaUrls.length - 1 ? 0 : i + 1,
                    );
                  }}
                  className="absolute right-2 top-1/2 -translate-y-1/2 bg-white/80  
                  text-black px-2 py-1 rounded-full"
                >
                  ›
                </button>

                {/* current image indicator */}
                <div className="absolute bottom-2 right-2 bg-black/60 text-white text-xs px-2 py-1 rounded">
                  {currentImageIndex + 1}/{post.mediaUrls.length}
                </div>
              </>
            )}
          </div>
        )}
      </div>
    </>
  );
}
