import { useState } from "react";

export default function ImagesCarousel({
  mediaUrls,
}: {
  mediaUrls?: string[];
}) {
  const [currentImageIndex, setCurrentImageIndex] = useState(0);

  if (mediaUrls === null || mediaUrls === undefined) return null;

  return (
    <div className="pt-10 flex flex-col gap-6">
      {mediaUrls.length > 1 && (
        <div className="flex flex-col gap-4 items-center">
          {/* prev and next button */}
          <div className="flex gap-4 justify-center text-gray-500">
            <button
              type="button"
              className="border border-gray-300 rounded-sm p-2 cursor-pointer"
              onClick={() => {
                setCurrentImageIndex((i) =>
                  i === 0 ? mediaUrls.length - 1 : i - 1,
                );
              }}
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
                strokeWidth={1.5}
                stroke="currentColor"
                className="size-6"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="M6.75 15.75 3 12m0 0 3.75-3.75M3 12h18"
                />
              </svg>
            </button>
            <button
              type="button"
              className="border border-gray-300 rounded-sm p-2 cursor-pointer"
              onClick={() => {
                setCurrentImageIndex((i) =>
                  i === mediaUrls.length - 1 ? 0 : i + 1,
                );
              }}
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
                strokeWidth={1.5}
                stroke="currentColor"
                className="size-6"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="M17.25 8.25 21 12m0 0-3.75 3.75M21 12H3"
                />
              </svg>
            </button>
          </div>

          {/* current index */}
          <div className="flex gap-2 items-center">
            {mediaUrls.map((_, index) => {
              return (
                <span
                  key={index}
                  className={`${index == currentImageIndex ? "bg-gray-500  " : "bg-gray-300 "} h-2 w-2 rounded-full`}
                ></span>
              );
            })}
          </div>
        </div>
      )}

      <img
        src={mediaUrls[currentImageIndex]}
        alt={mediaUrls[currentImageIndex]}
        className="object-cover w-full aspect-4/3 "
      />
    </div>
  );
}
