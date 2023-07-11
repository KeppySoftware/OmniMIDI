// source_location standard header (core)

// Copyright (c) Microsoft Corporation.
// SPDX-License-Identifier: Apache-2.0 WITH LLVM-exception
// OM dev here, I had to copypaste it in a new file to make it work...

#pragma once
#ifndef _SOURCE_LOCATION_
#define _SOURCE_LOCATION_

#include <yvals_core.h>
#include <cstdint>

namespace OmniMIDI {
    struct source_location {
        _NODISCARD static consteval source_location current(const uint_least32_t _Line_ = __builtin_LINE(),
            const uint_least32_t _Column_ = __builtin_COLUMN(), const char* const _File_ = __builtin_FILE(),
            const char* const _Function_ = __builtin_FUNCTION()) noexcept {
            source_location _Result{};
            _Result._Line = _Line_;
            _Result._Column = _Column_;
            _Result._File = _File_;
            _Result._Function = _Function_;
            return _Result;
        }

        _NODISCARD_CTOR constexpr source_location() noexcept = default;

        _NODISCARD constexpr uint_least32_t line() const noexcept {
            return _Line;
        }
        _NODISCARD constexpr uint_least32_t column() const noexcept {
            return _Column;
        }
        _NODISCARD constexpr const char* file_name() const noexcept {
            return _File;
        }
        _NODISCARD constexpr const char* function_name() const noexcept {
            return _Function;
        }

    private:
        uint_least32_t _Line{};
        uint_least32_t _Column{};
        const char* _File = "";
        const char* _Function = "";
    };
}

#endif
