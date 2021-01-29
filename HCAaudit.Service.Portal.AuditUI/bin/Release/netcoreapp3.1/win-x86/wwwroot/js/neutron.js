/* eslint-disable */
"use strict";

var _this = void 0;

/* ==========================================================================
   Neutron Button Groups - Toggle Buttons
   ========================================================================== */

/* defines and assigns all dropdown tabs */
var buttonToggles = document.querySelectorAll(
  ".neu-button-toggles .neu-button-toggles__button"
);
/* iterates over all buttons in node list */

Object.keys(buttonToggles).map(function (b) {
  /* adds event listener to buttons */
  buttonToggles[b].addEventListener("click", handleButtonToggle);
});
/* removes button class */

function buttonOff() {
  Object.keys(buttonToggles).map(function (t) {
    buttonToggles[t].classList.remove("neu-button-toggles__button-active");
  });
}
/* toggles button class */

function handleButtonToggle(e) {
  // clears active button
  buttonOff();
  e.currentTarget.classList.toggle("neu-button-toggles__button-active");
}
/* ==========================================================================
   Neutron Chip
   ========================================================================== */

var chips = document.querySelectorAll(".neu-chip--interact");
Object.keys(chips).map(function (c) {
  chips[c].addEventListener("click", handleChipClick);
});

function handleChipClick(e) {
  /* if the target is the action button it removes chip */
  if (e.target.className.includes("neu-chip__action")) {
    this.parentNode.removeChild(this);
  }
}
/* ================================================================================
  Dropdown
  ================================================================================ */

/* defines and assigns all dropdown tabs */

var dropdownTabs = document.querySelectorAll(
  ".neu-dropdown--tab .neu-tablist__tab"
);
/* iterates over all dropdown tabs in node list */

Object.keys(dropdownTabs).map(function (t) {
  /* adds event listener to each tab that has a dropdown */
  dropdownTabs[t].addEventListener("click", handleDropdownClick);
});
/* defines function that toggles visibility of dropdown content */

function handleDropdownClick(e) {
  // clears all active dropdowns
  removeActiveDropdowns();
  e.currentTarget.nextElementSibling.classList.toggle(
    "neu-dropdown__is-active"
  );
}
/* removes visibility class of dropdown content
 * when user clicks off of the menu
 */

document.addEventListener("mouseup", function (e) {
  var isTab = e.target.classList.contains(".neu-tablist__tab");

  if (!isTab) {
    removeActiveDropdowns();
  }
});

function removeActiveDropdowns() {
  var dropdownContent = document.querySelectorAll(
    ".neu-dropdown--tab .neu-dropdown__content"
  );
  Object.keys(dropdownContent).map(function (c) {
    dropdownContent[c].classList.remove("neu-dropdown__is-active");
  });
}

document.onkeydown = function (evt) {
  evt = evt || window.event;

  if (evt.keyCode == 27) {
    removeActiveDropdowns();
  }
};
/* ========================================================================
  File Uploader
  ======================================================================== */

(function () {
  var InputFile = function InputFile(element) {
    this.element = element;
    this.input = this.element.getElementsByClassName("neu-uploader__input")[0];
    this.label = this.element.getElementsByClassName("neu-uploader__label")[0];
    this.multipleUpload = this.input.hasAttribute("multiple"); // allows for multiple files selection
    // when user selects a file, neu-uploader__text will populate with the uploaded file's name

    this.labelText = this.element.getElementsByClassName(
      "neu-uploader__text"
    )[0];
    this.initialLabel = this.labelText.textContent;
    initInputFileEvents(this);
  };

  function initInputFileEvents(inputFile) {
    // makes label focusable
    inputFile.label.setAttribute("tabindex", "0");
    inputFile.input.setAttribute("tabindex", "-1"); // triggered when file is selected or the file picker modal is closed

    inputFile.input.addEventListener("focusin", function (event) {
      inputFile.label.focus();
    }); // 'enter' key triggers file selection

    inputFile.label.addEventListener("keydown", function (event) {
      if (
        (event.keyCode && event.keyCode == 13) ||
        (event.key && event.key.toLowerCase() == "enter")
      ) {
        inputFile.input.click();
      }
    }); // file has been selected -> update label text

    inputFile.input.addEventListener("change", function (event) {
      updateInputLabelText(inputFile);
    });
  }

  function updateInputLabelText(inputFile) {
    var label = "";

    if (inputFile.input.files && inputFile.input.files.length < 1) {
      label = inputFile.initialLabel; // no selection -> revert to initial label
    } else if (
      inputFile.multipleUpload &&
      inputFile.input.files &&
      inputFile.input.files.length > 1
    ) {
      label = inputFile.input.files.length + " files"; // multiple selection -> show number of files
    } else {
      label = inputFile.input.value.split("\\").pop(); // single file selection -> show name of the file
    }

    inputFile.labelText.textContent = label;
  } //initialize the InputFile objects

  var inputFiles = document.getElementsByClassName("neu-uploader");

  if (inputFiles.length > 0) {
    for (var i = 0; i < inputFiles.length; i++) {
      (function (i) {
        new InputFile(inputFiles[i]);
      })(i);
    }
  }
})();
/* ==========================================================================
   Navigation - Mobile
   ========================================================================== */
// Switches the Menu Icon onclick

function mobileMenuIconSwitch() {
  var menuIcon = document.getElementById("neu-mobile-nav__icon-switch");

  if (menuIcon.innerHTML === "menu") {
    menuIcon.innerHTML = "close";
    openMobileNav();
  } else {
    menuIcon.innerHTML = "menu";
    closeMobileNav();
  }
}

function openMobileNav() {
  document.querySelector(".neu-mobile-nav").style.width = "100%";
}

function closeMobileNav() {
  document.querySelector(".neu-mobile-nav").style.width = "0";
}

var mobNav = document.querySelector(".neu-mobile-nav");
var mobNavParents = [];
var mobNavMenus = [];

if (mobNav) {
  mobNav.addEventListener("click", function (event) {
    if (typeof event.target.dataset.childMenu !== "undefined") {
      var childMenu = getChildMenu(event.target);
      showMenu(childMenu);
      event.preventDefault();
    }

    if (event.target.classList.contains("neu-nav-mobile__button_back")) {
      resetMenu();
    }
  });
}

function getChildMenu(element) {
  var selector = element.dataset.childMenu;
  var childMenu = document.querySelector(selector);
  return childMenu;
}

function showMenu(menu) {
  mobNavParents.push(menu.parentElement);
  mobNavMenus.push(menu);
  menu.remove();
  menu.style.transform = "translateX(100%)";
  mobNav.appendChild(menu);
  setTimeout(reveal, 0);
}

function reveal() {
  var menu = mobNavMenus[mobNavMenus.length - 1];
  var prev = menu.previousElementSibling;
  menu.style.transform = "translateX(0)";
  prev.style.transform = "translateX(-100%)";
}

function resetMenu() {
  if (mobNavParents.length === 0 || mobNavMenus.length === 0) {
    return;
  }

  var menu = mobNavMenus[mobNavMenus.length - 1];
  var prev = menu.previousElementSibling;
  menu.style.transform = "translateX(100%)";
  prev.style.transform = "translateX(0)";
  mobNav.addEventListener("transitionend", hide, {
    once: true,
  });
}

function hide() {
  var menu = mobNavMenus.pop();
  var parent = mobNavParents.pop();
  menu.remove();
  parent.appendChild(menu);
}
/* ================================================================================
   POLYFILLS JS
   ================================================================================ */

/* ==============================
   NodeList FOREACH
   ============================== */

if (window.NodeList && !NodeList.prototype.forEach) {
  NodeList.prototype.forEach = Array.prototype.forEach;
}
/* ==============================
   Element CLOSEST
   ============================== */

if (!Element.prototype.matches) {
  Element.prototype.matches =
    Element.prototype.msMatchesSelector ||
    Element.prototype.webkitMatchesSelector;
}

if (!Element.prototype.closest) {
  Element.prototype.closest = function (s) {
    var el = _this;

    do {
      if (el.matches(s)) return el;
      el = el.parentElement || el.parentNode;
    } while (el !== null && el.nodeType === 1);

    return null;
  };
}
/* eslint-disable no-tabs */

/* ==========================================================================
   Neutron Sidenav - Tabbed
   ========================================================================== */

function switchTab(event, tabList) {
  var i, tabContent, tabs; // variables
  // Hide tab content

  tabContent = document.getElementsByClassName("neu-sidenav--tabbed__list");

  for (i = 0; i < tabContent.length; i++) {
    tabContent[i].style.display = "none";
  } // Remove tab "active" class

  tabs = document.getElementsByClassName("neu-tablist__tab");

  for (i = 0; i < tabs.length; i++) {
    tabs[i].className = tabs[i].className.replace(
      " neu-tablist__tab-on-light--is-active",
      ""
    );
  } // Show current tab content and assign "active"

  document.getElementById(tabList).style.display = "block";
  event.currentTarget.className += " neu-tablist__tab-on-light--is-active";
}
/* ================================================================================
   UTILS JS
   ================================================================================ */
// traverses dom nodes until it finds parent that has the desired class

function findParentWithClass(el, cls) {
  while (el.parentNode) {
    el = el.parentNode;

    if (el.classList.contains(cls)) {
      return el;
    }
  }

  return null;
}
